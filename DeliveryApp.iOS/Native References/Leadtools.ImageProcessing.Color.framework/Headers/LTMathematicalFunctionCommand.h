//
//  LTMathematicalFunctionCommand.h
//  Leadtools.ImageProcessing.Color
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

typedef NS_ENUM(NSInteger, LTMathematicalFunctionCommandType) {
    LTMathematicalFunctionCommandTypeSquare     = 0x0000,
    LTMathematicalFunctionCommandTypeLogarithm  = 0x0001,
    LTMathematicalFunctionCommandTypeSquareRoot = 0x0002,
    LTMathematicalFunctionCommandTypeSine       = 0x0003,
    LTMathematicalFunctionCommandTypeCosine     = 0x0004
};

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTMathematicalFunctionCommand : LTRasterCommand

@property (nonatomic, assign) LTMathematicalFunctionCommandType type;
@property (nonatomic, assign) NSUInteger factor;

- (instancetype)initWithType:(LTMathematicalFunctionCommandType)type factor:(NSUInteger)factor NS_DESIGNATED_INITIALIZER;

@end
