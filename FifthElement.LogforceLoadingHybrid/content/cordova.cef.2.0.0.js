//

// File generated at :: Thu Jul 26 2012 10:02:03 GMT+0300 (FLE Daylight Time)

/*
 Licensed to the Apache Software Foundation (ASF) under one
 or more contributor license agreements.  See the NOTICE file
 distributed with this work for additional information
 regarding copyright ownership.  The ASF licenses this file
 to you under the Apache License, Version 2.0 (the
 "License"); you may not use this file except in compliance
 with the License.  You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing,
 software distributed under the License is distributed on an
 "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 KIND, either express or implied.  See the License for the
 specific language governing permissions and limitations
 under the License.
 */


;(function() {

// file: lib\scripts\require.js
    var require,
        define;

    (function () {
        var modules = {};

        function build(module) {
            var factory = module.factory;
            module.exports = {};
            delete module.factory;
            factory(require, module.exports, module);
            return module.exports;
        }

        require = function (id) {
            if (!modules[id]) {
                throw "module " + id + " not found";
            }
            return modules[id].factory ? build(modules[id]) : modules[id].exports;
        };

        define = function (id, factory) {
            if (modules[id]) {
                throw "module " + id + " already defined";
            }

            modules[id] = {
                id: id,
                factory: factory
            };
        };

        define.remove = function (id) {
            delete modules[id];
        };

    })();

//Export for use in node
    if (typeof module === "object" && typeof require === "function") {
        module.exports.require = require;
        module.exports.define = define;
    }
// file: lib/cordova.js
    define("cordova", function(require, exports, module) {
        var channel = require('cordova/channel');

        /**
         * Listen for DOMContentLoaded and notify our channel subscribers.
         */
        document.addEventListener('DOMContentLoaded', function() {
            channel.onDOMContentLoaded.fire();
        }, false);
        if (document.readyState == 'complete' || document.readyState == 'interactive') {
            channel.onDOMContentLoaded.fire();
        }

        /**
         * Intercept calls to addEventListener + removeEventListener and handle deviceready,
         * resume, and pause events.
         */
        var m_document_addEventListener = document.addEventListener;
        var m_document_removeEventListener = document.removeEventListener;
        var m_window_addEventListener = window.addEventListener;
        var m_window_removeEventListener = window.removeEventListener;

        /**
         * Houses custom event handlers to intercept on document + window event listeners.
         */
        var documentEventHandlers = {},
            windowEventHandlers = {};

        document.addEventListener = function(evt, handler, capture) {
            var e = evt.toLowerCase();
            if (typeof documentEventHandlers[e] != 'undefined') {
                if (evt === 'deviceready') {
                    documentEventHandlers[e].subscribeOnce(handler);
                } else {
                    documentEventHandlers[e].subscribe(handler);
                }
            } else {
                m_document_addEventListener.call(document, evt, handler, capture);
            }
        };

        window.addEventListener = function(evt, handler, capture) {
            var e = evt.toLowerCase();
            if (typeof windowEventHandlers[e] != 'undefined') {
                windowEventHandlers[e].subscribe(handler);
            } else {
                m_window_addEventListener.call(window, evt, handler, capture);
            }
        };

        document.removeEventListener = function(evt, handler, capture) {
            var e = evt.toLowerCase();
            // If unsubcribing from an event that is handled by a plugin
            if (typeof documentEventHandlers[e] != "undefined") {
                documentEventHandlers[e].unsubscribe(handler);
            } else {
                m_document_removeEventListener.call(document, evt, handler, capture);
            }
        };

        window.removeEventListener = function(evt, handler, capture) {
            var e = evt.toLowerCase();
            // If unsubcribing from an event that is handled by a plugin
            if (typeof windowEventHandlers[e] != "undefined") {
                windowEventHandlers[e].unsubscribe(handler);
            } else {
                m_window_removeEventListener.call(window, evt, handler, capture);
            }
        };

        function createEvent(type, data) {
            var event = document.createEvent('Events');
            event.initEvent(type, false, false);
            if (data) {
                for (var i in data) {
                    if (data.hasOwnProperty(i)) {
                        event[i] = data[i];
                    }
                }
            }
            return event;
        }

        if(typeof window.console === "undefined") {
            window.console = {
                log:function(){}
            };
        }

        var cordova = {
            define:define,
            require:require,
            /**
             * Methods to add/remove your own addEventListener hijacking on document + window.
             */
            addWindowEventHandler:function(event, opts) {
                return (windowEventHandlers[event] = channel.create(event, opts));
            },
            addDocumentEventHandler:function(event, opts) {
                return (documentEventHandlers[event] = channel.create(event, opts));
            },
            removeWindowEventHandler:function(event) {
                delete windowEventHandlers[event];
            },
            removeDocumentEventHandler:function(event) {
                delete documentEventHandlers[event];
            },
            /**
             * Retreive original event handlers that were replaced by Cordova
             *
             * @return object
             */
            getOriginalHandlers: function() {
                return {'document': {'addEventListener': m_document_addEventListener, 'removeEventListener': m_document_removeEventListener},
                    'window': {'addEventListener': m_window_addEventListener, 'removeEventListener': m_window_removeEventListener}};
            },
            /**
             * Method to fire event from native code
             */
            fireDocumentEvent: function(type, data) {
                var evt = createEvent(type, data);
                if (typeof documentEventHandlers[type] != 'undefined') {
                    setTimeout(function() {
                        documentEventHandlers[type].fire(evt);
                    }, 0);
                } else {
                    document.dispatchEvent(evt);
                }
            },
            fireWindowEvent: function(type, data) {
                var evt = createEvent(type,data);
                if (typeof windowEventHandlers[type] != 'undefined') {
                    setTimeout(function() {
                        windowEventHandlers[type].fire(evt);
                    }, 0);
                } else {
                    window.dispatchEvent(evt);
                }
            },
            // TODO: this is Android only; think about how to do this better
            shuttingDown:false,
            UsePolling:false,
            // END TODO

            // TODO: iOS only
            // This queue holds the currently executing command and all pending
            // commands executed with cordova.exec().
            commandQueue:[],
            // Indicates if we're currently in the middle of flushing the command
            // queue on the native side.
            commandQueueFlushing:false,
            // END TODO
            /**
             * Plugin callback mechanism.
             */
            callbackId: 0,
            callbacks:  {},
            callbackStatus: {
                NO_RESULT: 0,
                OK: 1,
                CLASS_NOT_FOUND_EXCEPTION: 2,
                ILLEGAL_ACCESS_EXCEPTION: 3,
                INSTANTIATION_EXCEPTION: 4,
                MALFORMED_URL_EXCEPTION: 5,
                IO_EXCEPTION: 6,
                INVALID_ACTION: 7,
                JSON_EXCEPTION: 8,
                ERROR: 9
            },

            /**
             * Called by native code when returning successful result from an action.
             *
             * @param callbackId
             * @param args
             */
            callbackSuccess: function(callbackId, args) {
                if (cordova.callbacks[callbackId]) {

                    // If result is to be sent to callback
                    if (args.status == cordova.callbackStatus.OK) {
                        try {
                            if (cordova.callbacks[callbackId].success) {
                                cordova.callbacks[callbackId].success(args.message);
                            }
                        }
                        catch (e) {
                            console.log("Error in success callback: "+callbackId+" = "+e);
                        }
                    }

                    // Clear callback if not expecting any more results
                    if (!args.keepCallback) {
                        delete cordova.callbacks[callbackId];
                    }
                }
            },

            /**
             * Called by native code when returning error result from an action.
             *
             * @param callbackId
             * @param args
             */
            callbackError: function(callbackId, args) {
                if (cordova.callbacks[callbackId]) {
                    try {
                        if (cordova.callbacks[callbackId].fail) {
                            cordova.callbacks[callbackId].fail(args.message);
                        }
                    }
                    catch (e) {
                        console.log("Error in error callback: "+callbackId+" = "+e);
                    }

                    // Clear callback if not expecting any more results
                    if (!args.keepCallback) {
                        delete cordova.callbacks[callbackId];
                    }
                }
            },
            addConstructor: function(func) {
                channel.onCordovaReady.subscribeOnce(function() {
                    try {
                        func();
                    } catch(e) {
                        console.log("Failed to run constructor: " + e);
                    }
                });
            }
        };

// Register pause, resume and deviceready channels as events on document.
        channel.onPause = cordova.addDocumentEventHandler('pause');
        channel.onResume = cordova.addDocumentEventHandler('resume');
        channel.onDeviceReady = cordova.addDocumentEventHandler('deviceready');

        module.exports = cordova;

    });

// file: lib\common\builder.js
    define("cordova/builder", function(require, exports, module) {
        var utils = require('cordova/utils');

        function each(objects, func, context) {
            for (var prop in objects) {
                if (objects.hasOwnProperty(prop)) {
                    func.apply(context, [objects[prop], prop]);
                }
            }
        }

        function include(parent, objects, clobber, merge) {
            each(objects, function (obj, key) {
                try {
                    var result = obj.path ? require(obj.path) : {};

                    if (clobber) {
                        // Clobber if it doesn't exist.
                        if (typeof parent[key] === 'undefined') {
                            parent[key] = result;
                        } else if (typeof obj.path !== 'undefined') {
                            // If merging, merge properties onto parent, otherwise, clobber.
                            if (merge) {
                                recursiveMerge(parent[key], result);
                            } else {
                                parent[key] = result;
                            }
                        }
                        result = parent[key];
                    } else {
                        // Overwrite if not currently defined.
                        if (typeof parent[key] == 'undefined') {
                            parent[key] = result;
                        } else if (merge && typeof obj.path !== 'undefined') {
                            // If merging, merge parent onto result
                            recursiveMerge(result, parent[key]);
                            parent[key] = result;
                        } else {
                            // Set result to what already exists, so we can build children into it if they exist.
                            result = parent[key];
                        }
                    }

                    if (obj.children) {
                        include(result, obj.children, clobber, merge);
                    }
                } catch(e) {
                    utils.alert('Exception building cordova JS globals: ' + e + ' for key "' + key + '"');
                }
            });
        }

        /**
         * Merge properties from one object onto another recursively.  Properties from
         * the src object will overwrite existing target property.
         *
         * @param target Object to merge properties into.
         * @param src Object to merge properties from.
         */
        function recursiveMerge(target, src) {
            for (var prop in src) {
                if (src.hasOwnProperty(prop)) {
                    if (typeof target.prototype !== 'undefined' && target.prototype.constructor === target) {
                        // If the target object is a constructor override off prototype.
                        target.prototype[prop] = src[prop];
                    } else {
                        target[prop] = typeof src[prop] === 'object' ? recursiveMerge(
                            target[prop], src[prop]) : src[prop];
                    }
                }
            }
            return target;
        }

        module.exports = {
            build: function (objects) {
                return {
                    intoButDontClobber: function (target) {
                        include(target, objects, false, false);
                    },
                    intoAndClobber: function(target) {
                        include(target, objects, true, false);
                    },
                    intoAndMerge: function(target) {
                        include(target, objects, true, true);
                    }
                };
            }
        };

    });

// file: lib\common\channel.js
    define("cordova/channel", function(require, exports, module) {
        var utils = require('cordova/utils');

        /**
         * Custom pub-sub "channel" that can have functions subscribed to it
         * This object is used to define and control firing of events for
         * cordova initialization.
         *
         * The order of events during page load and Cordova startup is as follows:
         *
         * onDOMContentLoaded         Internal event that is received when the web page is loaded and parsed.
         * onNativeReady              Internal event that indicates the Cordova native side is ready.
         * onCordovaReady             Internal event fired when all Cordova JavaScript objects have been created.
         * onCordovaInfoReady         Internal event fired when device properties are available.
         * onCordovaConnectionReady   Internal event fired when the connection property has been set.
         * onDeviceReady              User event fired to indicate that Cordova is ready
         * onResume                   User event fired to indicate a start/resume lifecycle event
         * onPause                    User event fired to indicate a pause lifecycle event
         * onDestroy                  Internal event fired when app is being destroyed (User should use window.onunload event, not this one).
         *
         * The only Cordova events that user code should register for are:
         *      deviceready           Cordova native code is initialized and Cordova APIs can be called from JavaScript
         *      pause                 App has moved to background
         *      resume                App has returned to foreground
         *
         * Listeners can be registered as:
         *      document.addEventListener("deviceready", myDeviceReadyListener, false);
         *      document.addEventListener("resume", myResumeListener, false);
         *      document.addEventListener("pause", myPauseListener, false);
         *
         * The DOM lifecycle events should be used for saving and restoring state
         *      window.onload
         *      window.onunload
         *
         */

        /**
         * Channel
         * @constructor
         * @param type  String the channel name
         * @param opts  Object options to pass into the channel, currently
         *                     supports:
         *                     onSubscribe: callback that fires when
         *                       something subscribes to the Channel. Sets
         *                       context to the Channel.
         *                     onUnsubscribe: callback that fires when
         *                       something unsubscribes to the Channel. Sets
         *                       context to the Channel.
         */
        var Channel = function(type, opts) {
                this.type = type;
                this.handlers = {};
                this.numHandlers = 0;
                this.guid = 1;
                this.fired = false;
                this.enabled = true;
                this.events = {
                    onSubscribe:null,
                    onUnsubscribe:null
                };
                if (opts) {
                    if (opts.onSubscribe) this.events.onSubscribe = opts.onSubscribe;
                    if (opts.onUnsubscribe) this.events.onUnsubscribe = opts.onUnsubscribe;
                }
            },
            channel = {
                /**
                 * Calls the provided function only after all of the channels specified
                 * have been fired.
                 */
                join: function (h, c) {
                    var i = c.length;
                    var len = i;
                    var f = function() {
                        if (!(--i)) h();
                    };
                    for (var j=0; j<len; j++) {
                        !c[j].fired?c[j].subscribeOnce(f):i--;
                    }
                    if (!i) h();
                },
                create: function (type, opts) {
                    channel[type] = new Channel(type, opts);
                    return channel[type];
                },

                /**
                 * cordova Channels that must fire before "deviceready" is fired.
                 */
                deviceReadyChannelsArray: [],
                deviceReadyChannelsMap: {},

                /**
                 * Indicate that a feature needs to be initialized before it is ready to be used.
                 * This holds up Cordova's "deviceready" event until the feature has been initialized
                 * and Cordova.initComplete(feature) is called.
                 *
                 * @param feature {String}     The unique feature name
                 */
                waitForInitialization: function(feature) {
                    if (feature) {
                        var c = null;
                        if (this[feature]) {
                            c = this[feature];
                        }
                        else {
                            c = this.create(feature);
                        }
                        this.deviceReadyChannelsMap[feature] = c;
                        this.deviceReadyChannelsArray.push(c);
                    }
                },

                /**
                 * Indicate that initialization code has completed and the feature is ready to be used.
                 *
                 * @param feature {String}     The unique feature name
                 */
                initializationComplete: function(feature) {
                    var c = this.deviceReadyChannelsMap[feature];
                    if (c) {
                        c.fire();
                    }
                }
            };

        function forceFunction(f) {
            if (f === null || f === undefined || typeof f != 'function') throw "Function required as first argument!";
        }

        /**
         * Subscribes the given function to the channel. Any time that
         * Channel.fire is called so too will the function.
         * Optionally specify an execution context for the function
         * and a guid that can be used to stop subscribing to the channel.
         * Returns the guid.
         */
        Channel.prototype.subscribe = function(f, c, g) {
            // need a function to call
            forceFunction(f);

            var func = f;
            if (typeof c == "object") { func = utils.close(c, f); }

            g = g || func.observer_guid || f.observer_guid;
            if (!g) {
                // first time we've seen this subscriber
                g = this.guid++;
            }
            else {
                // subscriber already handled; dont set it twice
                return g;
            }
            func.observer_guid = g;
            f.observer_guid = g;
            this.handlers[g] = func;
            this.numHandlers++;
            if (this.events.onSubscribe) this.events.onSubscribe.call(this);
            if (this.fired) func.call(this);
            return g;
        };

        /**
         * Like subscribe but the function is only called once and then it
         * auto-unsubscribes itself.
         */
        Channel.prototype.subscribeOnce = function(f, c) {
            // need a function to call
            forceFunction(f);

            var g = null;
            var _this = this;
            var m = function() {
                f.apply(c || null, arguments);
                _this.unsubscribe(g);
            };
            if (this.fired) {
                if (typeof c == "object") { f = utils.close(c, f); }
                f.apply(this, this.fireArgs);
            } else {
                g = this.subscribe(m);
            }
            return g;
        };

        /**
         * Unsubscribes the function with the given guid from the channel.
         */
        Channel.prototype.unsubscribe = function(g) {
            // need a function to unsubscribe
            if (g === null || g === undefined) { throw "You must pass _something_ into Channel.unsubscribe"; }

            if (typeof g == 'function') { g = g.observer_guid; }
            var handler = this.handlers[g];
            if (handler) {
                if (handler.observer_guid) handler.observer_guid=null;
                this.handlers[g] = null;
                delete this.handlers[g];
                this.numHandlers--;
                if (this.events.onUnsubscribe) this.events.onUnsubscribe.call(this);
            }
        };

        /**
         * Calls all functions subscribed to this channel.
         */
        Channel.prototype.fire = function(e) {
            if (this.enabled) {
                var fail = false;
                this.fired = true;
                for (var item in this.handlers) {
                    var handler = this.handlers[item];
                    if (typeof handler == 'function') {
                        var rv = (handler.apply(this, arguments)===false);
                        fail = fail || rv;
                    }
                }
                this.fireArgs = arguments;
                return !fail;
            }
            return true;
        };

// defining them here so they are ready super fast!
// DOM event that is received when the web page is loaded and parsed.
        channel.create('onDOMContentLoaded');

// Event to indicate the Cordova native side is ready.
        channel.create('onNativeReady');

// Event to indicate that all Cordova JavaScript objects have been created
// and it's time to run plugin constructors.
        channel.create('onCordovaReady');

// Event to indicate that device properties are available
        channel.create('onCordovaInfoReady');

// Event to indicate that the connection property has been set.
        channel.create('onCordovaConnectionReady');

// Event to indicate that Cordova is ready
        channel.create('onDeviceReady');

// Event to indicate a resume lifecycle event
        channel.create('onResume');

// Event to indicate a pause lifecycle event
        channel.create('onPause');

// Event to indicate a destroy lifecycle event
        channel.create('onDestroy');

// Channels that must fire before "deviceready" is fired.
        channel.waitForInitialization('onCordovaReady');
        channel.waitForInitialization('onCordovaConnectionReady');

        module.exports = channel;

    });

// file: lib\common\common.js
    define("cordova/common", function(require, exports, module) {
        module.exports = {
            objects: {
                cordova: {
                    path: 'cordova',
                    children: {
                        exec: {
                            path: 'cordova/exec'
                        }
                        //logger: {
                        //     path: 'cordova/plugin/logger'
                        // }
                    }
                },
                Cordova: {
                    children: {
                        exec: {
                            path: 'cordova/exec'
                        }
                    }
                },
                PhoneGap:{
                    children: {
                        exec: {
                            path: 'cordova/exec'
                        }
                    }
                }/*,
                 navigator: {
                 children: {
                 notification: {
                 path: 'cordova/plugin/notification'
                 },
                 accelerometer: {
                 path: 'cordova/plugin/accelerometer'
                 },
                 battery: {
                 path: 'cordova/plugin/battery'
                 },
                 camera:{
                 path: 'cordova/plugin/Camera'
                 },
                 compass:{
                 path: 'cordova/plugin/compass'
                 },
                 contacts: {
                 path: 'cordova/plugin/contacts'
                 },
                 device:{
                 children:{
                 capture: {
                 path: 'cordova/plugin/capture'
                 }
                 }
                 },
                 geolocation: {
                 path: 'cordova/plugin/geolocation'
                 },
                 network: {
                 children: {
                 connection: {
                 path: 'cordova/plugin/network'
                 }
                 }
                 },
                 splashscreen: {
                 path: 'cordova/plugin/splashscreen'
                 }
                 }
                 },
                 Acceleration: {
                 path: 'cordova/plugin/Acceleration'
                 },
                 Camera:{
                 path: 'cordova/plugin/CameraConstants'
                 },
                 CameraPopoverOptions: {
                 path: 'cordova/plugin/CameraPopoverOptions'
                 },
                 CaptureError: {
                 path: 'cordova/plugin/CaptureError'
                 },
                 CaptureAudioOptions:{
                 path: 'cordova/plugin/CaptureAudioOptions'
                 },
                 CaptureImageOptions: {
                 path: 'cordova/plugin/CaptureImageOptions'
                 },
                 CaptureVideoOptions: {
                 path: 'cordova/plugin/CaptureVideoOptions'
                 },
                 CompassHeading:{
                 path: 'cordova/plugin/CompassHeading'
                 },
                 CompassError:{
                 path: 'cordova/plugin/CompassError'
                 },
                 ConfigurationData: {
                 path: 'cordova/plugin/ConfigurationData'
                 },
                 Connection: {
                 path: 'cordova/plugin/Connection'
                 },
                 Contact: {
                 path: 'cordova/plugin/Contact'
                 },
                 ContactAddress: {
                 path: 'cordova/plugin/ContactAddress'
                 },
                 ContactError: {
                 path: 'cordova/plugin/ContactError'
                 },
                 ContactField: {
                 path: 'cordova/plugin/ContactField'
                 },
                 ContactFindOptions: {
                 path: 'cordova/plugin/ContactFindOptions'
                 },
                 ContactName: {
                 path: 'cordova/plugin/ContactName'
                 },
                 ContactOrganization: {
                 path: 'cordova/plugin/ContactOrganization'
                 },
                 Coordinates: {
                 path: 'cordova/plugin/Coordinates'
                 },
                 device: {
                 path: 'cordova/plugin/device'
                 },
                 DirectoryEntry: {
                 path: 'cordova/plugin/DirectoryEntry'
                 },
                 DirectoryReader: {
                 path: 'cordova/plugin/DirectoryReader'
                 },
                 Entry: {
                 path: 'cordova/plugin/Entry'
                 },
                 File: {
                 path: 'cordova/plugin/File'
                 },
                 FileEntry: {
                 path: 'cordova/plugin/FileEntry'
                 },
                 FileError: {
                 path: 'cordova/plugin/FileError'
                 },
                 FileReader: {
                 path: 'cordova/plugin/FileReader'
                 },
                 FileSystem: {
                 path: 'cordova/plugin/FileSystem'
                 },
                 FileTransfer: {
                 path: 'cordova/plugin/FileTransfer'
                 },
                 FileTransferError: {
                 path: 'cordova/plugin/FileTransferError'
                 },
                 FileUploadOptions: {
                 path: 'cordova/plugin/FileUploadOptions'
                 },
                 FileUploadResult: {
                 path: 'cordova/plugin/FileUploadResult'
                 },
                 FileWriter: {
                 path: 'cordova/plugin/FileWriter'
                 },
                 Flags: {
                 path: 'cordova/plugin/Flags'
                 },
                 LocalFileSystem: {
                 path: 'cordova/plugin/LocalFileSystem'
                 },
                 Media: {
                 path: 'cordova/plugin/Media'
                 },
                 MediaError: {
                 path: 'cordova/plugin/MediaError'
                 },
                 MediaFile: {
                 path: 'cordova/plugin/MediaFile'
                 },
                 MediaFileData:{
                 path: 'cordova/plugin/MediaFileData'
                 },
                 Metadata:{
                 path: 'cordova/plugin/Metadata'
                 },
                 Position: {
                 path: 'cordova/plugin/Position'
                 },
                 PositionError: {
                 path: 'cordova/plugin/PositionError'
                 },
                 ProgressEvent: {
                 path: 'cordova/plugin/ProgressEvent'
                 },
                 requestFileSystem:{
                 path: 'cordova/plugin/requestFileSystem'
                 },
                 resolveLocalFileSystemURI:{
                 path: 'cordova/plugin/resolveLocalFileSystemURI'
                 }*/
            }
        };

    });

// file: lib\cef\exec.js
    define("cordova/exec", function(require, exports, module) {
        /*global module*/
        var cordova = require('cordova');

        /**
         * Execute a cordova command.  It is up to the native side whether this action
         * is synchronous or asynchronous.  The native side can return:
         *      Synchronous: PluginResult object as a JSON string
         *      Asynchrounous: Empty string ""
         * If async, the native side will cordova.callbackSuccess or cordova.callbackError,
         * depending upon the result of the action.
         *
         * @param {Function} success    The success callback
         * @param {Function} fail       The fail callback
         * @param {String} service      The name of the service to use
         * @param {String} action       Action to be run in cordova
         * @param {String[]} [args]     Zero or more arguments to pass to the method

         */
        module.exports = function(success, fail, service, action, args) {

            var callbackId = service + cordova.callbackId++;

            if (typeof success === "function" || typeof fail === "function") {
                cordova.callbacks[callbackId] = {success:success, fail:fail};
            }
            // generate a new command string, ex. DebugConsole/log/DebugConsole23/["wtf dude?"]
            var command = service + "/" + action + "/" + callbackId + "/" + JSON.stringify(args);
            // pass it on to Notify
            try {
                if(window.external) {
                    window.external.notify(command);
                }
                else {
                    console.log("window.external not available :: command=" + command);
                }
            }
            catch(e) {
                console.log("Exception calling native with command :: " + command + " :: exception=" + e);
            }
        };


    });

// file: lib\cef\platform.js
    define("cordova/platform", function(require, exports, module) {
        /*global module*/
        module.exports = {
            id:"cef",
            initialize: function() {

                var channel = require('cordova/channel');
                channel.onCordovaInfoReady.fire();
                channel.onCordovaConnectionReady.fire();
            },

            objects: {
                CordovaCommandResult: {
                    path:"cordova/plugin/cef/cordovacommandresult"
                }
            }
        };
    });

// file: lib\cef\plugin\cef\cordovacommandresult.js
    define("cordova/plugin/cef/cordovacommandresult", function(require, exports, module) {
        var channel = require('cordova/channel'),
            cordova = require('cordova');

        module.exports = function(status,callbackId,args,cast) {

            var parsedArgs;

            //The statuses copied from WP7 implementation
            //Not currently used in the CEF native component, but leaving
            //them here for possible future use.
            if(status === "backbutton") {
                cordova.fireDocumentEvent("backbutton");
                return "true";
            }
            else if(status === "resume") {
                cordova.fireDocumentEvent('resume');
                return "true";
            }
            else if(status === "pause") {
                cordova.fireDocumentEvent('pause');
                return "true";
            }


            try {
                parsedArgs = typeof args === "string" ? JSON.parse(args) : args;
            }
            catch(ex) {
                console.log("Parse error in CordovaCommandResult :: " + ex);
                return;
            }

            try {
                // For some commands, the message is a JSON encoded string
                // and other times, it is just a string, the try/catch handles the
                // case where message was indeed, just a string.
                parsedArgs.message = JSON.parse(parsedArgs.message);
            }
            catch(ex) {

            }

            var safeStatus = parseInt(status, 10);
            if(safeStatus === cordova.callbackStatus.NO_RESULT ||
                safeStatus === cordova.callbackStatus.OK) {
                cordova.callbackSuccess(callbackId,parsedArgs,cast);
            }
            else {
                cordova.callbackError(callbackId,parsedArgs,cast);
            }
        };
    });

// file: lib\common\utils.js
    define("cordova/utils", function(require, exports, module) {
        var utils = exports;

        /**
         * Returns an indication of whether the argument is an array or not
         */
        utils.isArray = function(a) {
            return Object.prototype.toString.call(a) == '[object Array]';
        };

        /**
         * Returns an indication of whether the argument is a Date or not
         */
        utils.isDate = function(d) {
            return Object.prototype.toString.call(d) == '[object Date]';
        };

        /**
         * Does a deep clone of the object.
         */
        utils.clone = function(obj) {
            if(!obj || typeof obj == 'function' || utils.isDate(obj) || typeof obj != 'object') {
                return obj;
            }

            var retVal, i;

            if(utils.isArray(obj)){
                retVal = [];
                for(i = 0; i < obj.length; ++i){
                    retVal.push(utils.clone(obj[i]));
                }
                return retVal;
            }

            retVal = {};
            for(i in obj){
                if(!(i in retVal) || retVal[i] != obj[i]) {
                    retVal[i] = utils.clone(obj[i]);
                }
            }
            return retVal;
        };

        /**
         * Returns a wrappered version of the function
         */
        utils.close = function(context, func, params) {
            if (typeof params == 'undefined') {
                return function() {
                    return func.apply(context, arguments);
                };
            } else {
                return function() {
                    return func.apply(context, params);
                };
            }
        };

        /**
         * Create a UUID
         */
        utils.createUUID = function() {
            return UUIDcreatePart(4) + '-' +
                UUIDcreatePart(2) + '-' +
                UUIDcreatePart(2) + '-' +
                UUIDcreatePart(2) + '-' +
                UUIDcreatePart(6);
        };

        /**
         * Extends a child object from a parent object using classical inheritance
         * pattern.
         */
        utils.extend = (function() {
            // proxy used to establish prototype chain
            var F = function() {};
            // extend Child from Parent
            return function(Child, Parent) {
                F.prototype = Parent.prototype;
                Child.prototype = new F();
                Child.__super__ = Parent.prototype;
                Child.prototype.constructor = Child;
            };
        }());

        /**
         * Alerts a message in any available way: alert or console.log.
         */
        utils.alert = function(msg) {
            if (alert) {
                alert(msg);
            } else if (console && console.log) {
                console.log(msg);
            }
        };

        /**
         * Formats a string and arguments following it ala sprintf()
         *
         * see utils.vformat() for more information
         */
        utils.format = function(formatString /* ,... */) {
            var args = [].slice.call(arguments, 1);
            return utils.vformat(formatString, args);
        };

        /**
         * Formats a string and arguments following it ala vsprintf()
         *
         * format chars:
         *   %j - format arg as JSON
         *   %o - format arg as JSON
         *   %c - format arg as ''
         *   %% - replace with '%'
         * any other char following % will format it's
         * arg via toString().
         *
         * for rationale, see FireBug's Console API:
         *    http://getfirebug.com/wiki/index.php/Console_API
         */
        utils.vformat = function(formatString, args) {
            if (formatString === null || formatString === undefined) return "";
            if (arguments.length == 1) return formatString.toString();
            if (typeof formatString != "string") return formatString.toString();

            var pattern = /(.*?)%(.)(.*)/;
            var rest    = formatString;
            var result  = [];

            while (args.length) {
                var arg   = args.shift();
                var match = pattern.exec(rest);

                if (!match) break;

                rest = match[3];

                result.push(match[1]);

                if (match[2] == '%') {
                    result.push('%');
                    args.unshift(arg);
                    continue;
                }

                result.push(formatted(arg, match[2]));
            }

            result.push(rest);

            return result.join('');
        };

//------------------------------------------------------------------------------
        function UUIDcreatePart(length) {
            var uuidpart = "";
            for (var i=0; i<length; i++) {
                var uuidchar = parseInt((Math.random() * 256), 10).toString(16);
                if (uuidchar.length == 1) {
                    uuidchar = "0" + uuidchar;
                }
                uuidpart += uuidchar;
            }
            return uuidpart;
        }

//------------------------------------------------------------------------------
        function formatted(object, formatChar) {

            try {
                switch(formatChar) {
                    case 'j':
                    case 'o': return JSON.stringify(object);
                    case 'c': return '';
                }
            }
            catch (e) {
                return "error JSON.stringify()ing argument: " + e;
            }

            if ((object === null) || (object === undefined)) {
                return Object.prototype.toString.call(object);
            }

            return object.toString();
        }

    });


    window.cordova = require('cordova');

// file: lib\scripts\bootstrap.js
    (function (context) {
        var channel = require("cordova/channel"),
            _self = {
                boot: function () {
                    /**
                     * Create all cordova objects once page has fully loaded and native side is ready.
                     */
                    channel.join(function() {
                        var builder = require('cordova/builder'),
                            base = require('cordova/common'),
                            platform = require('cordova/platform');

                        // Drop the common globals into the window object, but be nice and don't overwrite anything.
                        builder.build(base.objects).intoButDontClobber(window);

                        // Drop the platform-specific globals into the window object
                        // and clobber any existing object.
                        builder.build(platform.objects).intoAndClobber(window);

                        // Merge the platform-specific overrides/enhancements into
                        // the window object.
                        if (typeof platform.merges !== 'undefined') {
                            builder.build(platform.merges).intoAndMerge(window);
                        }

                        // Call the platform-specific initialization
                        platform.initialize();

                        // Fire event to notify that all objects are created
                        channel.onCordovaReady.fire();

                        // Fire onDeviceReady event once all constructors have run and
                        // cordova info has been received from native side.
                        channel.join(function() {
                            require('cordova').fireDocumentEvent('deviceready');
                        }, channel.deviceReadyChannelsArray);

                    }, [ channel.onDOMContentLoaded, channel.onNativeReady ]);
                }
            };

        // boot up once native side is ready
        channel.onNativeReady.subscribeOnce(_self.boot);

        // _nativeReady is global variable that the native side can set
        // to signify that the native code is ready. It is a global since
        // it may be called before any cordova JS is ready.
        if (window._nativeReady) {
            channel.onNativeReady.fire();
        }

    }(window));


})();
