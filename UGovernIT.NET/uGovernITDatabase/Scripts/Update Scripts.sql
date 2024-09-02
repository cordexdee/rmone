-- Date: 12-Feb-2020, update value for StandardWorkItem column where it is Null in Ticket Hour table.
GO
Update TicketHours set StandardWorkItem=0 where StandardWorkItem is null;