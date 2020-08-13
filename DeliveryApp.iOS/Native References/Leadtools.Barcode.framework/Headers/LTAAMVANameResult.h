//
//  LTAAMVANameResult.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTLeadtools.h>

NS_ASSUME_NONNULL_BEGIN

LT_CLASS_AVAILABLE(10_10, 8_0, 20_0)
@interface LTAAMVANameResult : NSObject <NSCopying, NSSecureCoding>

@property (nonatomic, copy)   NSString *value;
@property (nonatomic, assign) BOOL inferredFromFullName;

@end

NS_ASSUME_NONNULL_END
