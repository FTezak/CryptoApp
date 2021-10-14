export interface Wallet {
    hasBinance: boolean;
    cryptos: CryptosWallet[];
    totalValue: number;
}

export interface CryptosWallet {
    amount: number;
    binanceAmount: number;
    cryptoApiReference: number;
    price: number;
    symbol: string;
    name: string;
}