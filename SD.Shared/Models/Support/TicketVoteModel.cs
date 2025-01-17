﻿namespace SD.Shared.Model.Support
{
    public class TicketVoteModel : DocumentBase
    {
        public TicketVoteModel() : base(DocumentType.TicketVote, false)
        {
        }

        public string? IdVotedUser { get; set; }

        public VoteType VoteType { get; set; }

#pragma warning disable S927 // Parameter names should match base declaration and other partial definitions
        public override void SetIds(string? TicketId)
#pragma warning restore S927 // Parameter names should match base declaration and other partial definitions
        {
            SetValues(Guid.NewGuid().ToString(), TicketId);
        }

        public override bool Equals(object? obj)
        {
            return obj is TicketVoteModel q && q.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }
    }

    public enum VoteType
    {
        MinusOne = -1,
        PlusOne = 1
    }
}