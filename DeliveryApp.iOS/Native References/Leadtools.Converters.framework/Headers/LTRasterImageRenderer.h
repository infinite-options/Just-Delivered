//
//  LTRasterImageRenderer.h
//  Leadtools.Converters
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <CoreGraphics/CGContext.h>

#import <Leadtools.Converters/LTRasterImageRenderOptions.h>

#import <Leadtools/LTLeadtools.h>
#import <Leadtools/LTRasterImage.h>

NS_ASSUME_NONNULL_BEGIN

LT_CLASS_AVAILABLE(10_10, 8_0, 20_0)
@interface LTRasterImageRenderer : NSObject

+ (BOOL)render:(LTRasterImage *)image context:(CGContextRef __null_unspecified)context destinationRect:(CGRect)destinationRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;
+ (BOOL)render:(LTRasterImage *)image context:(CGContextRef)context sourceRect:(CGRect)sourceRect destinationRect:(CGRect)destinationRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;
+ (BOOL)render:(LTRasterImage *)image context:(CGContextRef __null_unspecified)context sourceRect:(CGRect)sourceRect sourceClipRect:(CGRect)sourceClipRect destinationRect:(CGRect)destinationRect destinationClipRect:(CGRect)destinationClipRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;

+ (BOOL)renderRegion:(LTRasterImage *)image context:(CGContextRef __null_unspecified)context sourceRect:(CGRect)sourceRect sourceClipRect:(CGRect)sourceClipRect destinationRect:(CGRect)destinationRect destinationClipRect:(CGRect)destinationClipRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;

+ (BOOL)renderOverlay:(LTRasterImage *)image context:(CGContextRef)context sourceRect:(CGRect)sourceRect sourceClipRect:(CGRect)sourceClipRect destinationRect:(CGRect)destinationRect destinationClipRect:(CGRect)destinationClipRect options:(nullable LTRasterImageRenderOptions *)options overlayIndex:(NSInteger)overlayIndex error:(NSError **)error;

+ (BOOL)renderBuffer:(NSData *)dataBuffer row:(NSInteger)row count:(NSInteger)count image:(LTRasterImage *)image context:(CGContextRef __null_unspecified)context sourceRect:(CGRect)sourceRect sourceClipRect:(CGRect)sourceClipRect destinationRect:(CGRect)destinationRect destinationClipRect:(CGRect)destinationClipRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;
+ (BOOL)renderRegionBuffer:(NSData *)dataBuffer row:(NSInteger)row count:(NSInteger)count image:(LTRasterImage *)image context:(CGContextRef __null_unspecified)context sourceRect:(CGRect)sourceRect sourceClipRect:(CGRect)sourceClipRect destinationRect:(CGRect)destinationRect destinationClipRect:(CGRect)destinationClipRect options:(nullable LTRasterImageRenderOptions *)options error:(NSError **)error;

- (instancetype)init NS_UNAVAILABLE;

@end

NS_ASSUME_NONNULL_END
