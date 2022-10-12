import { Component, OnInit } from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup} from "@angular/forms";
import {DataService} from "../services/data.service";
import {OrdersByGrid} from "../models/OrderByGrid";

@Component({
  selector: 'app-orders-by-grid',
  templateUrl: './orders-by-grid.component.html',
  styleUrls: ['./orders-by-grid.component.scss']
})
export class OrdersByGridComponent implements OnInit {
  form: UntypedFormGroup;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private dataService:DataService
  ) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.form = this.formBuilder.group({
      pair: ["", []],
      side: ["", []],
      amount: ["", []],
      ordersCount: ["", []],
      minPrice: ["", []],
      maxPrice: ["", []],
      stopPrice: ["", []],
      stopLimitPrice: ["", []],

    })
  }

  submit() {
    var cmd = new OrdersByGrid();

    cmd.pair = this.form.get("pair").value;
    cmd.side = this.form.get("side").value;
    cmd.amount = +this.form.get("amount").value;
    cmd.ordersCount = +this.form.get("ordersCount").value;
    cmd.minPrice = +this.form.get("minPrice").value;
    cmd.maxPrice = +this.form.get("maxPrice").value;
    cmd.stopPrice = +this.form.get("stopPrice").value;
    cmd.stopLimitPrice = +this.form.get("stopLimitPrice").value;

    this.dataService.ordersByGrid(cmd).subscribe(res=>{
      console.log(res)
    })
  }

}
