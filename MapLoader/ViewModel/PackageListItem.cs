using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FifthElement.Kotka.MapPackageManager.Model;
using FifthElement.Kotka.MapPackageManager.Model.Operations;
using FifthElement.Kotka.MapPackageManager.Util;
using FifthElement.MapLoader.View;

namespace FifthElement.MapLoader.ViewModel
{
    public class PackageListItem
    {
        private LoadingStatus _status;

        public enum LoadingStatus
        {
            NotStarted,
            Verifying,
            Verified,
            Loading,
            Success,
            Error,
            Failure
        }

        public PackageListItemView View { get; private set; }
        public MapPackageOperation Model { get; private set; }

        public PackageListItem(MapPackageOperation model)
        {
            this.Model = model;
            this.View = new PackageListItemView();
            this.SetStatus(LoadingStatus.NotStarted);

            this.View.SetPackageName(model.DisplayName);
        }

        public void SetStatus(LoadingStatus status)
        {
            View.SetStatusIndicatorImage(GetStatusIndicatorImage(status));
            View.SetPackageSize(GetFileSizeDisplayText(status));
            View.SetActive(status == LoadingStatus.Loading);
            View.SetTooltip(GetTooltipText(status));

            this._status = status;

        }
        public void SetDownloadStatusText(string statusText)
        {
            View.SetPackageSize(statusText);
        }

        private string GetTooltipText(LoadingStatus status)
        {

            switch (status)
            {
                case LoadingStatus.NotStarted:
                    if (Model.Operation == MapPackageOperation.OperationType.LocalInstallation)
                        return "Paikallinen paketti";

                    if (Model.Operation == MapPackageOperation.OperationType.OnlineInstallation)
                        return "Online-paketti";

                    if (Model.Operation == MapPackageOperation.OperationType.Removal)
                        return "Poistettava paketti";

                    throw new ArgumentOutOfRangeException("Model.Operation");

                case LoadingStatus.Verifying:
                    return "Vahvistetaan...";
                case LoadingStatus.Verified:
                    return "Jonossa";
                case LoadingStatus.Loading:
                    return "Ladataan...";
                case LoadingStatus.Success:
                    return "Valmis";
                case LoadingStatus.Error:
                    return "Virhe. Yritetään hetken kuluttua uudestaan.";
                case LoadingStatus.Failure:
                    return "Paketin lataus epäonnistui.";
                default:
                    throw new ArgumentOutOfRangeException("status");
            }                
        }

        private string GetFileSizeDisplayText(LoadingStatus status)
        {
            switch (status)
            {
                case LoadingStatus.NotStarted:
                    return "";

                case LoadingStatus.Verifying:
                    return "Haetaan...";

                case LoadingStatus.Verified:
                case LoadingStatus.Loading:
                case LoadingStatus.Success:
                case LoadingStatus.Error:


                    if (Model.Operation == MapPackageOperation.OperationType.Removal)
                        return "(poisto)";

                    return FileSizeUtil.Humanize(Model.ActualSize); 

                case LoadingStatus.Failure:
                    return "Epäonnistui!";

                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }
        private Image GetStatusIndicatorImage(LoadingStatus status)
        {
            switch (status)
            {
                case LoadingStatus.NotStarted:
                case LoadingStatus.Verified:

                    if (Model.Operation == MapPackageOperation.OperationType.LocalInstallation)
                        return Properties.Resources.status_notstarted_local;

                    if (Model.Operation == MapPackageOperation.OperationType.OnlineInstallation)
                        return Properties.Resources.status_notstarted_web;

                    if (Model.Operation == MapPackageOperation.OperationType.Removal)
                        return Properties.Resources.status_delete;

                    throw new ArgumentOutOfRangeException("Model.Operation");

                case LoadingStatus.Verifying:
                    return Properties.Resources.status_loading_transparent;
                case LoadingStatus.Loading:
                    return Properties.Resources.status_loading_transparent;
                case LoadingStatus.Success:
                    return Properties.Resources.status_success;
                case LoadingStatus.Error:
                    return Properties.Resources.status_fail;
                case LoadingStatus.Failure:
                    return Properties.Resources.status_fail;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }    
        }

        public bool Verify()
        {
            return Model.VerifyPackage();
        }
    }
}
