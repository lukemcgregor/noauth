'use strict';

import React from 'react';
import update from 'react-addons-update';
import _ from 'lodash';
import Select from 'react-select';
require('react-select/dist/react-select.css');

export default class LoginForm extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			claimedIdentifier: this.guid(),
			loading: false,
			claims: []
		};		
		this.addClaim = this.addClaim.bind(this);		
		this.removeClaim = this.removeClaim.bind(this);		
		this.claimedIdentifierChanged = this.claimedIdentifierChanged.bind(this);		
		this.claimTypeChanged = this.claimTypeChanged.bind(this);		
		this.claimValueChanged = this.claimValueChanged.bind(this);
		this.populateRandomUser = this.populateRandomUser.bind(this);

	}
	populateRandomUser(){
		this.setState({loading:true});
		var xhr = new XMLHttpRequest();
		xhr.open('GET', 'https://randomuser.me/api/', true);		
		xhr.setRequestHeader("Content-type", "application/json");
		xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
		xhr.onload = (() => {
			var claims = [];
			_.forEach(this.props.availableClaims, (x)=>{
				if(x.Default){
					var val = JSON.parse(xhr.responseText).results[0][x.Name.toLowerCase()];
					if(typeof val === 'object'){
						if(val.first){
							val = val.first + ' ' + val.last;
						}
						else{
							val = '';
						}
					}

					claims.push({type: x.Value, value:val });
				}
			});
			this.setState({claims: claims, loading:false});
		}).bind(this);
		xhr.send();
	}
	guid() {
		function s4() {
			return Math.floor((1 + Math.random()) * 0x10000)
			  .toString(16)
			  .substring(1);
		}
		return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
		  s4() + '-' + s4() + s4() + s4();
	}
	addClaim() {
		var claims = this.state.claims.slice(0);
		claims.push({type: '', value:'' });
		this.setState({claims: claims});
	}
	removeClaim(e, index){
	}	

	claimedIdentifierChanged(e){
		this.setState({claimedIdentifier: e.target.value});
	}		
	claimTypeChanged(index,value){
		var claims = update(this.state.claims, {
			[index]: {type:{$set: value }}
		});
		this.setState({claims: claims});
	}	
	claimValueChanged(index,e){		
		var claims = update(this.state.claims, {
			[index]: {value:{$set: e.target.value }}
		});
		this.setState({claims: claims});
	}
	renderClaims() {
		if(this.state.loading){
			return (<span className='loading'>Loading fake user</span>);
		}

		return this.state.claims.map((claim, index)=>
		{
			return (<div className='form-group' key={index}>
				<div className="col-md-offset-1 col-md-5">
					<Select 
		name={`Claims[${index}].Type`} 
		className='claim-type' 
		placeholder='Claim Type' 
		value={claim.type} 
		allowCreate
		addLabelText='{label}'
		onChange={this.claimTypeChanged.bind(this,index)} 
		options={_.map(this.props.availableClaims,(x)=>{return { value: x.Value, label: x.Value } })} />
</div>
<div className="col-md-5">
	<input className='claim-value' name={`Claims[${index}].Value`} className='form-control' placeholder='Claim Value' value={claim.value} onChange={this.claimValueChanged.bind(this,index)} />
				</div>
			</div>);
});
}
	render() {
		return (
			<form className='form-horizontal' method='post'>
				<div className='form-group'>
					<label className='col-md-6 control-label'>Claimed Identitfier</label>
					<div className="col-md-5">
						<input id='claimed-identifier' name='ClaimedIdentifier' className='form-control' value={this.state.claimedIdentifier} onChange={this.claimedIdentifierChanged} />
					</div>
				</div>
{this.renderClaims()}
				<div className='col-md-offset-1 col-md-10' style={{textAlign: 'right'}}>
					<a href='#' onClick={this.addClaim}>Add claim</a>
				</div>
				<div className="form-group" style={{ marginTop: '40px'}}>
					<div className="col-md-6" style={{textAlign: 'right'}}> 
						<button id='populate-random-user' type="button" className="btn btn-lg btn-default" disabled={this.state.loading} onClick={this.populateRandomUser} title="Thanks to randomuser.me">Populate random user</button>
					</div>
					<div className="col-md-6"> 
						<button id='login' type="submit" className="btn btn-lg btn-success" disabled={this.state.loading}>Log in</button>
					</div>
				</div>
			</form>
		);
}
}