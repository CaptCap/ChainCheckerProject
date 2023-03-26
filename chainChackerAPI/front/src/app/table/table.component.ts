import { Component } from '@angular/core';
import { ChaincheckService } from '../chaincheck.service';


@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent {
  constructor(public service:ChaincheckService) {

  }
  checkStatus(hash:string){
    this.service.saveTransactionStatus(hash)

  }

}
