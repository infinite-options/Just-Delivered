//
//  LTGaussianCommand.h
//  Leadtools.ImageProcessing.Effects
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTGaussianCommand : LTRasterCommand

@property (nonatomic, assign) NSInteger radius;

- (instancetype)initWithRadius:(NSInteger)radius NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
