//
//  LTKeyStoneCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>
#import <Leadtools/LTRasterImage.h>
#import <Leadtools/LTPrimitives.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTKeyStoneCommand : LTRasterCommand

@property (nonatomic, strong, nullable) LTRasterImage *transformedImage;
@property (nonatomic, strong, nullable) NSArray<NSValue *> *polygonPoints; //LeadPoint

- (instancetype)initWithPolygonPoints:(NSArray<NSValue *> *)polygonPoints /*LeadPoint*/ NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
