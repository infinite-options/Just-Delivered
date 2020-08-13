//
//  LTRasterImageConverter.h
//  Leadtools.Converters
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#if __has_include(<UIKit/UIImage.h>)
#   import <UIKit/UIImage.h>
#endif // #if __has_include(<UIKit/UIImage.h>)

#import <Leadtools/LTRasterImage.h>

#import <Leadtools.Converters/LTConvertersDefines.h>
#import <Leadtools.Converters/LTConvertToImageData.h>
#import <Leadtools.Converters/LTConvertFromImageData.h>

#if __has_include(<Metal/Metal.h>)
#  import <Metal/Metal.h>
#endif

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTRasterImageConverter : NSObject

@property (class, atomic, strong) dispatch_queue_t asyncQueue; // Default is Concurrent
@property (class, atomic, strong) dispatch_group_t asyncGroup;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
#if __has_include(<UIKit/UIImage.h>)

+ (nullable UIImage *)convertToImage:(LTRasterImage *)rasterImage options:(LTConvertToImageOptions)options error:(NSError **)error NS_SWIFT_NAME(convertToImage(_:options:));
+ (void)convertToImageAsync:(LTRasterImage *)rasterImage options:(LTConvertToImageOptions)options completion:(void (^)(UIImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convertToImage(_:options:completion:));

+ (nullable UIImage *)convertToImage:(LTRasterImage *)rasterImage data:(nullable LTConvertToImageData *)data error:(NSError **)error NS_SWIFT_NAME(convertToImage(_:data:));
+ (void)convertToImageAsync:(LTRasterImage *)rasterImage data:(nullable LTConvertToImageData *)data completion:(void (^)(UIImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convertToImage(_:data:completion:));

+ (nullable LTRasterImage *)convertFromImage:(UIImage *)image options:(LTConvertFromImageOptions)options error:(NSError **)error NS_SWIFT_NAME(convert(from:options:));
+ (void)convertFromImageAsync:(UIImage *)image options:(LTConvertFromImageOptions)options completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convert(from:options:completion:));

+ (nullable LTRasterImage *)convertFromImage:(UIImage *)image data:(nullable LTConvertFromImageData *)data error:(NSError **)error NS_SWIFT_NAME(convert(from:data:));
+ (void)convertFromImageAsync:(UIImage *)image data:(nullable LTConvertFromImageData *)data completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convert(from:data:completion:));

#endif // #if __has_include(<UIKit/UIImage.h>)
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

+ (nullable CGImageRef)convertToCGImage:(LTRasterImage *)rasterImage options:(LTConvertToImageOptions)options error:(NSError **)error NS_SWIFT_NAME(convertToCGImage(_:options:));
+ (void)convertToCGImageAsync:(LTRasterImage *)rasterImage options:(LTConvertToImageOptions)options completion:(void (^)(CGImageRef __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convertToCGImage(_:data:completion:));

+ (nullable CGImageRef)convertToCGImage:(LTRasterImage *)rasterImage data:(nullable LTConvertToImageData *)data error:(NSError **)error NS_SWIFT_NAME(convertToCGImage(_:data:));
+ (void)convertToCGImageAsync:(LTRasterImage *)rasterImage data:(nullable LTConvertToImageData *)data completion:(void (^)(CGImageRef __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convertToCGImage(_:data:completion:));

+ (nullable LTRasterImage *)convertFromCGImage:(CGImageRef)image options:(LTConvertFromImageOptions)options error:(NSError **)error NS_SWIFT_NAME(convert(from:options:));
+ (void)convertFromCGImageAsync:(CGImageRef)image options:(LTConvertFromImageOptions)options completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convert(from:options:completion:));

+ (nullable LTRasterImage *)convertFromCGImageSource:(CGImageSourceRef)image options:(LTConvertFromImageOptions)options error:(NSError **)error NS_SWIFT_NAME(convert(fromImageSource:options:));
+ (void)convertFromCGImageSourceAsync:(CGImageSourceRef)image options:(LTConvertFromImageOptions)options completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convert(fromImageSource:options:completion:));

+ (nullable LTRasterImage *)convertFromCGImage:(CGImageRef)image data:(nullable LTConvertFromImageData *)data error:(NSError **)error NS_SWIFT_NAME(convert(from:data:));
+ (void)convertFromCGImageAsync:(CGImageRef)image data:(nullable LTConvertFromImageData *)data completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_SWIFT_NAME(convert(from:data:completion:));

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

- (instancetype)init __unavailable;

@end

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#if __has_include(<Metal/Metal.h>)

// Since the deployment target of Leadtools.Converters for macOS is 10.10 but Metal wasn't available until 10.11, this suppresses any warnings that would be generated for "Partial Availablity"
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wpartial-availability"

@interface LTRasterImageConverter (Metal)

@property (class, atomic, weak, nullable) id<MTLCommandQueue> commandQueue NS_AVAILABLE(10_11, 8_0);

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

+ (nullable id<MTLTexture>)convertToMTLTexture:(LTRasterImage *)image device:(id<MTLDevice>)device format:(MTLPixelFormat)format resourceOptions:(MTLResourceOptions)resourceOptions options:(LTConvertToImageOptions)options error:(NSError **)error NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convertToMTLTexture(_:device:format:resourceOptions:options:));
+ (void)convertToMTLTextureAsync:(LTRasterImage *)image device:(id<MTLDevice>)device format:(MTLPixelFormat)format resourceOptions:(MTLResourceOptions)resourceOptions options:(LTConvertToImageOptions)options completion:(void (^)(id<MTLTexture> __nullable texture, NSError * __nullable error))completion NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convertToMTLTexture(_:device:format:resourceOptions:options:completion:));

+ (nullable LTRasterImage *)convertFromMTLTexture:(id<MTLTexture>)texture options:(LTConvertFromImageOptions)options error:(NSError **)error NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convert(from:options:));
+ (void)convertFromMTLTextureAsync:(id<MTLTexture>)texture options:(LTConvertFromImageOptions)options completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convert(from:options:completion:));

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

+ (nullable id<MTLBuffer>)convertToMTLBuffer:(LTRasterImage *)image device:(id<MTLDevice>)device resourceOptions:(MTLResourceOptions)resourceOptions options:(LTConvertToImageOptions)options error:(NSError **)error NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convertToMTLBuffer(_:device:resourceOptions:options:));
+ (void)convertToMTLBufferAsync:(LTRasterImage *)image device:(id<MTLDevice>)device resourceOptions:(MTLResourceOptions)resourceOptions options:(LTConvertToImageOptions)options completion:(void (^)(id<MTLBuffer> __nullable buffer, NSError * __nullable error))completion NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convertToMTLBuffer(_:device:resourceOptions:options:completion:));

+ (nullable LTRasterImage *)convertFromMTLBuffer:(id<MTLBuffer>)buffer offset:(NSUInteger)offset bytesPerRow:(NSUInteger)bytesPerRow width:(NSInteger)width height:(NSInteger)height bitsPerPixel:(NSInteger)bitsPerPixel order:(LTRasterByteOrder)order viewPerspective:(LTRasterViewPerspective)viewPerspective palette:(nullable NSArray<LTRasterColor*> *)palette options:(LTConvertFromImageOptions)options error:(NSError **)error NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convert(from:offset:bytesPerRow:width:height:bitsPerPixel:order:viewPerspective:palette:options:));
+ (void)convertFromMTLBufferAsync:(id<MTLBuffer>)buffer offset:(NSUInteger)offset bytesPerRow:(NSUInteger)bytesPerRow width:(NSInteger)width height:(NSInteger)height bitsPerPixel:(NSInteger)bitsPerPixel order:(LTRasterByteOrder)order viewPerspective:(LTRasterViewPerspective)viewPerspective palette:(nullable NSArray<LTRasterColor*> *)palette options:(LTConvertFromImageOptions)options completion:(void (^)(LTRasterImage * __nullable image, NSError * __nullable error))completion NS_AVAILABLE(10_11, 8_0) NS_SWIFT_NAME(convert(from:offset:bytesPerRow:width:height:bitsPerPixel:order:viewPerspective:palette:options:completion:));

@end

#pragma clang diagnostic pop

#endif // #if __has_include(<Metal/Metal.h>)

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

@interface LTRasterImage (LinkedImages)

#if __has_include(<UIKit/UIImage.h>)
@property (nonatomic, strong, readonly, nullable) UIImage *linkedImage;
#endif

@property (nonatomic, assign, readonly, nullable) CGImageRef linkedCGImage NS_RETURNS_INNER_POINTER CF_RETURNS_NOT_RETAINED;

#if __has_include(<Metal/Metal.h>)

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wpartial-availability"

@property (nonatomic, strong, readonly, nullable) id<MTLTexture> linkedMTLTexture NS_AVAILABLE(10_11, 8_0);
@property (nonatomic, strong, readonly, nullable) id<MTLBuffer> linkedMTLBuffer NS_AVAILABLE(10_11, 8_0);

#pragma clang diagnostic pop

#endif // #if __has_include(<Metal/Metal.h>)

@end

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

@interface LTRasterImageConverter (Deprecated)

#if __has_include(<UIKit/UIImage.h>)
+ (nullable UIImage *)getLinkedImage:(LTRasterImage *)image LT_DEPRECATED_USENEW(19_0, "LTRasterImage.linkedImage");
#endif

+ (nullable CGImageRef)getLinkedCGImage:(LTRasterImage *)image NS_RETURNS_INNER_POINTER CF_RETURNS_NOT_RETAINED LT_DEPRECATED_USENEW(19_0, "LTRasterImage.linkedCGImage");

@end

NS_ASSUME_NONNULL_END
