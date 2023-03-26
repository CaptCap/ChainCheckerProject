import { Component } from '@angular/core';
import { ChaincheckService } from '../chaincheck.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  constructor (public service:ChaincheckService ){}
  search(address:string){
    this.service.saveTransactions(address)
  }
  

}
