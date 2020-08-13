//
//  LTPatchCodeBarcodeWriteOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeWriteOptions.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTPatchCodeBarcodeWriteOptions : LTBarcodeWriteOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          BOOL useXModule;

@property (nonatomic, assign)          NSInteger xModule;

@end

NS_ASSUME_NONNULL_END
