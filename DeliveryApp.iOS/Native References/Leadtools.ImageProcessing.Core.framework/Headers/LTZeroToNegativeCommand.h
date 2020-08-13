//
//  LTZeroToNegativeCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTZeroToNegativeCommand : LTRasterCommand

@property (nonatomic, assign) NSInteger shiftAmount;
@property (nonatomic, assign) NSInteger minimumInput;
@property (nonatomic, assign) NSInteger maximumInput;
@property (nonatomic, assign) NSInteger minimumOutput;
@property (nonatomic, assign) NSInteger maximumOutput;

- (instancetype)initWithShiftAmount:(NSInteger)shiftAmount minimumInput:(NSInteger) minimumInput maximumInput:(NSInteger)maximumInput minimumOutput:(NSInteger)minimumOutput maximumOutput:(NSInteger)maximumOutput NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
