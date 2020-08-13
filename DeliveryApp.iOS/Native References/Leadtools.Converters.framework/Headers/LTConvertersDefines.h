//
//  LTConvertersDefines.h
//  Leadtools.Converters
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#if !defined(LTCONVERTERSDEFINES_H)
#define LTCONVERTERSDEFINES_H

#if defined(NS_OPTIONS)
typedef NS_OPTIONS(NSUInteger, LTConvertToImageOptions) {
    // No options
    LTConvertToImageOptionsNone = 0,
    
    // If the source raster image has alpha (is 32/64 bit), use them, otherwise, initialize to 0xFF if dest image has alpha
    LTConvertToImageOptionsKeepAlphaValues = 1 << 0,
    
    // Do not rotate the destination image. e.g. if source image is BottomLeft, the result image is flipped
    LTConvertToImageOptionsKeepViewPerspective = 1 << 1,
    
    // If source image is gray 12/16. Do not use the low/high bit values when converting scanlines
    LTConvertToImageOptionsIgnoreLowHighBitsOnGrayImages = 1 << 2,
    
    // With this option, the converted image and the LTRasterImage will share the same memory.
    // If the converted image is modified, you must call -[LTRasterImage didChange:] to synchronize
    // the changes with the LTRasterImage. The source LTRasterImage object maintains a strong reference
    // to the converted image until deallocated. Cannot be used with MTLResourceStorageModePrivate
    LTConvertToImageOptionsLinkImage = 1 << 3,
    
    // When converting to MTLTexture objects, use this flag to specify that the
    // MTLTexture should be mipmapped.
    LTConvertToImageOptionsMipmapped NS_ENUM_AVAILABLE(10_11, 8_0) = 1 << 4,
    
    // When converting to MTLTexture objects, use this flag (with LTConvertToImageOptionsMipmapped)
    // to specify that the MTLTexture should be mipmapped and that the mipmaps should be generated.
    LTConvertToImageOptionsGenerateMipmaps NS_ENUM_AVAILABLE(10_11, 8_0) = 1 << 5,
    
    // When converting to MTLTexture objects, use this flag to specify that the MTLTexture should
    // be backed by a MTLBuffer object. Cannot be combined with LTConvertToImageOptionsMipmapped
    // option.
    LTConvertToImageOptionsBufferBackedTexture NS_ENUM_AVAILABLE(NA, 8_0) = 1 << 6
};

typedef NS_OPTIONS(NSUInteger, LTConvertFromImageOptions) {
    // No options
    LTConvertFromImageOptionsNone = 0,
    
    // With this option, MTLResource objects and LTRasterImage will share the same memory.
    // ONLY valid for MTLTexture and MTLBuffer objects.
    LTConvertFromImageOptionsLinkImage = 1 << 0,
    
    LTConvertFromImageBlackIsZero = 1 << 1,
};

#else

typedef enum LTConvertToImageOptions {
    // No options
    LTConvertToImageOptionsNone = 0,
    
    // If the source raster image has alpha (is 32/64 bit), use them, otherwise, initialize to 0xFF if dest image has alpha
    LTConvertToImageOptionsKeepAlphaValues = 1 << 0,
    
    // Do not rotate the destination image. e.g. if source image is BottomLeft, the result image is flipped
    LTConvertToImageOptionsKeepViewPerspective = 1 << 1,
    
    // If source image is gray 12/16. Do not use the low/high bit values when converting scanlines
    LTConvertToImageOptionsIgnoreLowHighBitsOnGrayImages = 1 << 2,
    
    // With this option, the LTRasterImage and the source image will share the same memory.
    // If the source image is modified, you must call -[LTRasterImage onChanged:] to synchronize
    // the changes with the LTRasterImage. The LTRasterImage object maintains a strong reference
    // to the source image until deallocated.
    LTConvertToImageOptionsLinkImage = 1 << 3
} LTConvertToImageOptions;

typedef enum LTConvertFromImageOptions {
    // No options
    LTConvertFromImageOptionsNone = 0,
} LTConvertFromImageOptions;

#endif // #if defined(NS_OPTIONS)

#endif // #if !defined(LTCONVERTERSDEFINES_H)
