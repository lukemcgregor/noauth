import React from 'react';
import ReactDOM from 'react-dom';
import LoginForm from './LoginForm.jsx';

ReactDOM.render(React.createFactory(LoginForm)(), document.getElementById('login-form-mount-node'));