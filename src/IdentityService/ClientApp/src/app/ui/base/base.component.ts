import {OnDestroy} from "@angular/core";
import {Component} from "@angular/core";
import {UntypedFormGroup} from "@angular/forms";
import {Subscription} from "rxjs";
import {AppConstant} from "../../core/constants/app.constant";

@Component({
  selector: 'base',
  template: ""
})
export class BaseComponent implements OnDestroy {
  protected _subs: Subscription = new Subscription();
  form!: UntypedFormGroup;

  get controls() {
    return this.form.controls;
  }

  returnToClient() {
    location.assign(AppConstant.ReturnUrl);
  }

  ngOnDestroy() {
    console.log(this._subs)
    this._subs.unsubscribe();
  }

}
