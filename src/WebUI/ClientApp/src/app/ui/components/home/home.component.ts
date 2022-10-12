import {Component, OnInit} from '@angular/core';
import {TestService} from "../../../providers/services/test.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  data: any = ""

  constructor(private dataService:TestService) {
  }

  ngOnInit(): void {}

  getData() {
    this.dataService.GetData().subscribe((data) => {
      this.data = data;
    })
  }

}
