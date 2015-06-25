--==============================================================
-- Table: KEYVALUESTORAGE
--==============================================================
CREATE TABLE KEYVALUESTORAGE (
KEYVALUESTORAGEID    VARCHAR(32)	NOT NULL,
CREATED_BY           VARCHAR(32),
CREATED_ON           DATE			DEFAULT CURRENT_TIMESTAMP,
UPDATED_BY           VARCHAR(32),
UPDATED_ON           DATE,
STATUS               INTEGER		CHECK (STATUS IS NULL OR (STATUS IN (0,1,2,3))),
SYNCDATE             DATE,
INFO_TEXT            VARCHAR(1),
INFO_CODE            INTEGER,
DATA                 VARCHAR(1),
PRIMARY KEY (KEYVALUESTORAGEID)
);

--==============================================================
-- Index: KEYVALUESTORAGE_PK
--==============================================================
CREATE UNIQUE INDEX KEYVALUESTORAGE_PK ON KEYVALUESTORAGE (
KEYVALUESTORAGEID ASC
);

