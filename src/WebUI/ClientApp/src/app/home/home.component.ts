import {Component, OnInit} from '@angular/core';
import {DataService} from "../services/data.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  data: any = ""
  data2: any = ""

  constructor(private dataService:DataService) {
  }

  ngOnInit(): void {

  }

  getData() {
    this.dataService.GetData().subscribe((d) => {
      this.data = d;
    })
  }
  getData2() {
    this.dataService.GetData2().subscribe((d) => {
      this.data2 = d;
    })
  }

}
