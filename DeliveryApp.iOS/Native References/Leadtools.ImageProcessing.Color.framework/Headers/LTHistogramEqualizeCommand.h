//
//  LTHistogramEqualizeCommand.h
//  Leadtools.ImageProcessing.Color
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>
#import <Leadtools.ImageProcessing.Color/LTEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTHistogramEqualizeCommand : LTRasterCommand

@property (nonatomic, assign) LTHistogramEqualizeType type;

- (instancetype)initWithType:(LTHistogramEqualizeType)type NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
