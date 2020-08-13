//
//  LTLambdaConnectednessCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTLambdaConnectednessCommand : LTRasterCommand

@property (nonatomic, assign) NSInteger lambda;

- (instancetype)initWithLambda:(NSInteger)lambda NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
