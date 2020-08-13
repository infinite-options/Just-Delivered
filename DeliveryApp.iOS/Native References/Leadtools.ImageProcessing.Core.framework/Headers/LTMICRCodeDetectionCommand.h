//
//  LTMICRCodeDetectionCommand.h
//  Leadtools.ImageProcessing.Core
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTRasterCommand.h>
#import <Leadtools/LTPrimitives.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTMICRCodeDetectionCommand : LTRasterCommand

@property (nonatomic, assign)           LeadRect searchingZone;
@property (nonatomic, assign, readonly) LeadRect micrZone;

- (instancetype)initWithSearchingZone:(LeadRect)searchingZone NS_DESIGNATED_INITIALIZER;

@end

NS_ASSUME_NONNULL_END
