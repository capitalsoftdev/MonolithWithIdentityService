import {Component, OnInit} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {UntypedFormBuilder, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from "../../base/base.component";
import {IdentityService} from "../../../services/identity.service";
import {AuthModel} from "../../../core/models/auth.model";

@Component(
  {
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']
  })
export class LoginComponent extends BaseComponent implements OnInit {
  loading = false;
  loginFailed = false;
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
      rememberMe: [true, Validators.required]
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

    this.loginFailed = false;
    this.loading = true;

    const authModel = new AuthModel(
      this.controls.email.value,
      this.controls.password.value,
      this.controls.rememberMe.value,
      this.returnUrl
    );

    this.identityService.login(authModel)
      .subscribe({
        next: _ => {
          this.returnToClient()
        },
        error: error => {
          this.loginFailed = true;
          this.loading = false;
          console.log(error);
        }
      })

  }

}
