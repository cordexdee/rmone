CREATE FULLTEXT INDEX ON [dbo].[PMM]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_PMM]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;




GO
CREATE FULLTEXT INDEX ON [dbo].[NPR]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK__NPR__3214EC27D4A354F7]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;

GO
CREATE FULLTEXT INDEX ON [dbo].[TSR]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_TSR]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[ACR]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_ACR]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;

GO
CREATE FULLTEXT INDEX ON [dbo].[PRS]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_PRS]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[SVCRequests]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_SVCRequests]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[DRQ]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_DRQ]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[BTS]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_BTS]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[INC]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_INC]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[RCA]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_RCA]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[TSK]
    ([Title] LANGUAGE 0)
    KEY INDEX [PK_TSK]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;

Go
CREATE FULLTEXT INDEX ON [dbo].[Phrase]
  ([Phrase] LANGUAGE 0)
    KEY INDEX [PK_Phrase_PhraseId]
    ON [FullTextCatalog]
    WITH STOPLIST OFF;
