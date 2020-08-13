//
//  LTPostNetPlanetBarcodeReadOptions.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeReadOptions.h>
#import <Leadtools.Barcode/LTBarcodeReadEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTPostNetPlanetBarcodeReadOptions : LTBarcodeReadOptions

@property (nonatomic, copy, readonly)  NSString *friendlyName;

@property (nonatomic, assign)          LTBarcodeSearchDirection searchDirection;

@property (nonatomic, assign)          LTBarcodeReturnCheckDigit returnCheckDigit;

@property (nonatomic, assign)          NSInteger granularity;
@property (nonatomic, assign)          NSInteger whiteLinesNumber;

@end

NS_ASSUME_NONNULL_END
