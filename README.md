# StbImageResizeSharp
[![NuGet](https://img.shields.io/nuget/v/StbImageResizeSharp.svg)](https://www.nuget.org/packages/StbImageResizeSharp/) 
![Build & Publish](https://github.com/rds1983/StbImageResizeSharp/workflows/Build%20&%20Publish/badge.svg)
[![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

StbImageResizeSharp is C# port of the [stb_image_resize.h](https://github.com/nothings/stb/blob/master/stb_image_resize.h), which is C library to resize images.

# Adding Reference
There are two ways of referencing StbImageResizeSharp in the project:
1. Through nuget: https://www.nuget.org/packages/StbImageResizeSharp/
2. As submodule:
    
    a. `git submodule add https://github.com/rds1983/StbImageResizeSharp.git`
    
    b. Now there are two options:
       
      * Add src/StbImageResizeSharp.csproj to the solution
       
      * Include *.cs from folder "src" directly in the project. In this case, it might make sense to add StbImageResizeSharp_INTERNAL build compilation symbol to the project, so StbImageResizeSharp classes would become internal.
     
# Usage
StbImageResizeSharp exposes API similar to [stb_image_resize.h](https://github.com/nothings/stb/blob/master/stb_image_resize.h). 
Also adds some methods that pin input and output array.

Sample code to resize an image
```c# 
    StbImageResize.stbir_resize_uint8(imageData, width, height, image.Width * channels,
        newImageData, newWidth, newHeight, newWidth * channels, channels);
```

# License
Public Domain

# Credits
https://github.com/nothings/stb
