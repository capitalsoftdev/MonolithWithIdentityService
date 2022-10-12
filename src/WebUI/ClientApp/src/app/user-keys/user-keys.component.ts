import { Component, OnInit } from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup} from "@angular/forms";
import {DataService} from "../services/data.service";

@Component({
  selector: 'app-user-keys',
  templateUrl: './user-keys.component.html',
  styleUrls: ['./user-keys.component.scss']
})
export class UserKeysComponent implements OnInit {
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
      apiKey: ["", []],
      secretKey: ["", []],

    })
  }

  submit() {
    this.dataService.saveKeys(this.form.get("apiKey").value,this.form.get("secretKey").value).subscribe(res=>{
      console.log(res)
    })
  }
}
