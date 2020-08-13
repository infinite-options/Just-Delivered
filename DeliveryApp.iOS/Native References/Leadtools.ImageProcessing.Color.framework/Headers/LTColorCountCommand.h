//
//  LTColorCountCommand.h
//  Leadtools.ImageProcessing.Color
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTColorCountCommand : LTRasterCommand

@property (nonatomic, assign, readonly) NSUInteger colorCount;

@end
