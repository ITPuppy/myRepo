CREATE TABLE tblFriend (
    id      VARCHAR (15)  NOT NULL,
    groupId   VARCHAR (10)  NULL,
    groupName VARCHAR (100) NULL,
    friendId  VARCHAR (65535) NULL,
PRIMARY KEY (id,groupId)
);




CREATE TABLE tblGroup (
    groupId     VARCHAR (15)  NOT NULL,
    ownerId   VARCHAR (15)  NOT NULL,
    groupMember VARCHAR (65535) NULL,
    name       VARCHAR (100) NOT NULL,
    PRIMARY KEY (groupId)
);

CREATE TABLE tblMember(
    id          NVARCHAR (15)  NOT NULL,
    nickName    NVARCHAR (50)  NOT NULL,
    password    NVARCHAR (50)  NOT NULL,
    sex         NCHAR (10)     NULL,
    birthday    DATE           NULL,
    information NVARCHAR (65535) NULL,
    status      NVARCHAR (20)  NULL,
    PRIMARY KEY (id )
);

