export enum TransactionStatus{None, Normal,Suspicious, Danger, Error}
export interface ChainTransaction {
    hash: string;
    transactionType: string;
    date: string; //*DateTime
    from: string;
    to:string;
    value: string;
    status : TransactionStatus;

}