//
//  LTIDCountryDetectionCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>
#import <Leadtools/LTPrimitives.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTIDCountryDetectionCommand : LTRasterCommand

@property (nonatomic, strong, readonly, nullable) NSMutableArray<NSValue *> *charactersRects;

@property (nonatomic, assign)                     LeadRect countryRect;

@property (nonatomic, assign)                     NSInteger imageArea;

@end

NS_ASSUME_NONNULL_END
