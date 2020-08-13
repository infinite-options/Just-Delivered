//
//  LTMinMaxValuesCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTMinMaxValuesCommand : LTRasterCommand

@property (nonatomic, assign, readonly) NSInteger minimumValue;
@property (nonatomic, assign, readonly) NSInteger maximumValue;

@end
