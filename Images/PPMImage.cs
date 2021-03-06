﻿namespace Images
{
    public enum PPMImageType
    {
        P3,
        P6
    }
    public class PPMImage
    {
        public PPMImageType Type { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string ImageString { get; set; }
        public int BytesPerColor { get; set; }
        public int MaxValue { get; set; }
        public int[] Bits { get; set; }
        public int BitsCount { get; set; }
        public int MaxR { get; set; }
        public int MaxG { get; set; }
        public int MaxB { get; set; }
    }

}
