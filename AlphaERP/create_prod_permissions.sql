IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AlphaERP_ProdOrdersPermissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AlphaERP_ProdOrdersPermissions] (
        [CompNo]     SMALLINT     NOT NULL,
        [UserID]     VARCHAR (8)  NOT NULL,
        [ProgID]     INT          NOT NULL, -- 1: list stages, 2: Production, 3: Row Material
        [ProgAccess] BIT          DEFAULT 0,
        [ProgAdd]    BIT          DEFAULT 0,
        [ProgMod]    BIT          DEFAULT 0,
        [ProgDel]    BIT          DEFAULT 0,
        CONSTRAINT [PK_AlphaERP_ProdOrdersPermissions] PRIMARY KEY CLUSTERED ([CompNo] ASC, [UserID] ASC, [ProgID] ASC)
    );
END
GO
