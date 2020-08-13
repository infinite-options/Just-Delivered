//
//  LTCodecsPsdImageInfo.h
//  Leadtools.Codecs
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsPsdImageInfo : NSObject

@property (nonatomic, assign, readonly) NSInteger layers;
@property (nonatomic, assign, readonly) NSInteger channels;

- (instancetype)init __unavailable;

@end
