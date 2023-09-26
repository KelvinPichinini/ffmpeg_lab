﻿using System.Drawing;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Extensions.SkiaSharp;
using FFMpegCore.Extensions.System.Drawing.Common;
using FFMpegCore.Pipes;
using SkiaSharp;
using FFMpegImage = FFMpegCore.Extensions.System.Drawing.Common.FFMpegImage;
var now = DateTime.Now.ToString("s");
var inputPath = "./videos/input/sample-2.avi";
var outputPath = $"./videos/output/{now}.webm";


// {
//     FFMpegArguments
//         .FromFileInput(inputPath)
//         .OutputToFile(outputPath, false, options => options
//             .WithVideoCodec(VideoCodec.LibVpx)
//             .WithConstantRateFactor(21)
//             .WithAudioCodec(AudioCodec.Aac)
//             .WithVariableBitrate(4)
//             .WithVideoFilters(filterOptions => filterOptions
//                 .Scale(VideoSize.Hd))
//             .WithFastStart())
//         .ProcessSynchronously();
// }


var inputStream = new MemoryStream();
var outputStream = new MemoryStream();

{
    Console.WriteLine("Output MemoryStream Length: " + outputStream.Length);
    var proc1 = FFMpegArguments
        .FromFileInput(inputPath)
        .OutputToPipe(new StreamPipeSink(outputStream), options => options
            .WithVideoCodec(VideoCodec.LibVpx)
                .ForcePixelFormat("yuv420p")
                .ForceFormat("webm")
                .WithFastStart());
    
    Console.WriteLine("Output MemoryStream Length: " + outputStream.Length);


    now = DateTime.Now.ToString("s");
    outputPath = $"./videos/output/{now}.webm";
    Console.WriteLine("Output MemoryStream Length: " + outputStream.Length);
    // outputStream.Seek(0, SeekOrigin.Begin);
    var proc2 = FFMpegArguments
        .FromPipeInput(new StreamPipeSource(outputStream))
        .OutputToFile(outputPath, false, options => options
            .WithVideoCodec(VideoCodec.LibVpx)
            .ForceFormat("webm")
            .WithFastStart());

    proc1.ProcessAsynchronously();
    await proc2.ProcessAsynchronously();
}



