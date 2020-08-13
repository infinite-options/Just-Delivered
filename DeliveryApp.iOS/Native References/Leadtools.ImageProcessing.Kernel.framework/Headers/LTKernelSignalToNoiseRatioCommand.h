//
//  LTKernelSignalToNoiseRatioCommand.h
//  Leadtools.ImageProcessing.Kernel
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTKernelSignalToNoiseRatioCommand : LTRasterCommand

@property (nonatomic, assign, readonly) double ratio;

@end

NS_ASSUME_NONNULL_END
