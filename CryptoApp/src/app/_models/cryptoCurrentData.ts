export interface CryptoCurrentData {
    cryptoApiReference: number;
    symbol: string;
    name: string;
    date: Date;
    price: number; 
    dayPercentage: number;
    weekPercentage: number;
    monthPercentage: number;
    favorite: boolean;
}