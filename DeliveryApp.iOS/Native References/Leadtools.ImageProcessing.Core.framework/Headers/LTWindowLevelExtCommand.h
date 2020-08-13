//
//  LTWindowLevelExtCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTLeadtoolsDefines.h>
#import <Leadtools/LTRasterCommand.h>
#import <Leadtools/LTRasterColor16.h>

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTWindowLevelExtCommand : LTRasterCommand

@property (nonatomic, assign) NSInteger lowBit;
@property (nonatomic, assign) NSInteger highBit;
@property (nonatomic, assign) LTRasterByteOrder order;
@property (nonatomic, strong) NSMutableArray<LTRasterColor16 *> *lookupTable;

- (instancetype)initWithLookupTable:(NSArray<LTRasterColor16 *> *)lookupTable lowBit:(NSInteger)lowBit highBit:(NSInteger)highBit order:(LTRasterByteOrder)order NS_DESIGNATED_INITIALIZER;

@end
