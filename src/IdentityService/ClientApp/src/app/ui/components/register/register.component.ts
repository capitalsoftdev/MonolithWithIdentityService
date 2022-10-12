import {Component, OnInit} from '@angular/core';
import {UntypedFormBuilder, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {BaseComponent} from "../../base/base.component";
import {IdentityService} from "../../../services/identity.service";
import {AuthModel} from "../../../core/models/auth.model";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends BaseComponent implements OnInit {
  loading = false;
  registerFailed = false;
  returnUrl: string = "";

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    private activatedRoute: ActivatedRoute
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });

    // get return url from route parameters or default to '/'
    this.activatedRoute.queryParams.subscribe((data: any) => {
      this.returnUrl = data["ReturnUrl"];
    });

  }

  onSubmit() {

    if (this.form.invalid) {
      return;
    }

    this.registerFailed = false;
    this.loading = true;

    const authModel = new AuthModel(
      this.controls.email.value,
      this.controls.password.value,
      false,
      this.returnUrl
    );

    this.identityService.register(authModel)
      .subscribe({
        next: _ => {
          this.returnToClient("authorize")
        },
        error: error => {
          this.registerFailed = true;
          this.loading = false;
          console.log(error);
        }
      })

  }

}
