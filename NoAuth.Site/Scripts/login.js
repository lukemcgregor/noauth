import React from 'react';
import ReactDOM from 'react-dom';
import LoginForm from './LoginForm.jsx';

ReactDOM.render(React.createFactory(LoginForm)({ availableClaims: State.AvailiableClaimTypes }), document.getElementById('login-form-mount-node'));