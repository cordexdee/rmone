-- Date: 12-Feb-2020, Add default value on StandardWorkItem column for Ticket Hour table.
GO
ALTER TABLE TicketHours ADD CONSTRAINT DF_StandardWorkItem DEFAULT 0 FOR StandardWorkItem;