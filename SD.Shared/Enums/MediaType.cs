﻿namespace SD.Shared.Enums
{
    public enum MediaType
    {
        [Custom(Name = "movieName", ResourceType = typeof(Resources.Enum.MediaType))]
        movie = 1,

        [Custom(Name = "tvName", ResourceType = typeof(Resources.Enum.MediaType))]
        tv = 2
    }
}