export class AuthModel {
  email: string | undefined;
  password: string | undefined;
  rememberMe: boolean | undefined;
  returnUrl: string | undefined;

  constructor(email: string, password: string, rememberMe: boolean, returnUrl: string) {
    this.email = email;
    this.password = password;
    this.rememberMe = rememberMe;
    this.returnUrl = returnUrl;
  }
}
