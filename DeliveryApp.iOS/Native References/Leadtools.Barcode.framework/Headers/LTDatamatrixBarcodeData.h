//
//  LTDatamatrixBarcodeData.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools.Barcode/LTBarcodeData.h>
#import <Leadtools.Barcode/LTDatamatrixBarcodeEnums.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTDatamatrixBarcodeData : LTBarcodeData <NSCopying>

@property (nonatomic, assign) LTBarcodeSymbology symbology;

@property (nonatomic, assign) LTDatamatrixBarcodeSymbolSize symbolSize;

@end

NS_ASSUME_NONNULL_END
