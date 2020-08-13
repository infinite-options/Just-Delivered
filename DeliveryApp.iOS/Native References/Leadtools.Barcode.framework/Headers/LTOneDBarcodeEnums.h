//
//  LTOneDBarcodeEnums.h
//  Leadtools.Barcode
//
//  Copyright © 1991-2019 LEAD Technologies, Inc. All rights reserved.
//

typedef NS_ENUM(NSInteger, LTCode11BarcodeCheckDigitType) {
    LTCode11BarcodeCheckDigitTypeCDigit,
    LTCode11BarcodeCheckDigitTypeCAndKDigits
};

typedef NS_ENUM(NSInteger, LTMSIBarcodeModuloType) {
    LTMSIBarcodeModuloTypeModulo10,
    LTMSIBarcodeModuloTypeTwoModulo10,
    LTMSIBarcodeModuloTypeModulo11,
    LTMSIBarcodeModuloTypeModulo11And10
};

typedef NS_ENUM(NSInteger, LTCode128BarcodeTableEncoding) {
    LTCode128BarcodeTableEncodingAuto,
    LTCode128BarcodeTableEncodingATable,
    LTCode128BarcodeTableEncodingBTable,
    LTCode128BarcodeTableEncodingCTable
};
