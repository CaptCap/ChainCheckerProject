import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChainTransaction, TransactionStatus } from './entities/chainTransaction';



@Injectable({
  providedIn: 'root'
})
export class ChaincheckService {
  transactions:ChainTransaction[]=[
    {hash: '12345', transactionType: '12345',date: '11.01', from : 'sfdvsfv', to:'dsjjkjsc', value : '10000',status: TransactionStatus.Normal},
    
    {hash: '12345', transactionType: '12345',date: '11.01',from : 'sfdvsfv', to:'dsjjkjsc',value : '10000', status: TransactionStatus.Danger}
    ]

  constructor(public http:HttpClient) { }
  getTransactions(hash:string):Observable<ChainTransaction[]>{
    return this.http.get<ChainTransaction[]>('https://localhost:7222/analise/getByWallet/' + hash + '/1')
  }
  saveTransactions(hash:string){
    this.getTransactions(hash).subscribe(t=>this.transactions=t)
  }
  getTransactionStatus(hash:string):Observable<TransactionStatus>{
    return this.http.get<TransactionStatus>('https://localhost:7222/analise/getTransactionStatus/' + hash )
  }
  saveTransactionStatus(hash:string){
    this.getTransactionStatus(hash).subscribe(t=>this.transactions[this.transactions.findIndex(x=>x.hash===hash)].status=t)
  }

}
