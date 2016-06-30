'use strict';

import React from 'react';
import update from 'react-addons-update';

export default class LoginForm extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			claimedIdentifier: this.guid(),
			claims: []
		};		
		this.addClaim = this.addClaim.bind(this);		
		this.removeClaim = this.removeClaim.bind(this);		
		this.claimedIdentifierChanged = this.claimedIdentifierChanged.bind(this);		
		this.claimTypeChanged = this.claimTypeChanged.bind(this);		
		this.claimValueChanged = this.claimValueChanged.bind(this);
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
	claimTypeChanged(index,e){
		var claims = update(this.state.claims, {
			[index]: {type:{$set: e.target.value }}
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
		return this.state.claims.map((claim, index)=>
		{
			return (<div className='form-group' key={index}>
				<div className="col-md-offset-1 col-md-5">
					<input className='claim-type' name={`Claims[${index}].Type`} className='form-control' placeholder='Claim Type' value={claim.type} onChange={this.claimTypeChanged.bind(this,index)} />
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
				<div className="form-group">
					<div className="col-md-12" style={{textAlign: 'center', marginTop: '40px'}}> 
						<button type="submit" className="btn btn-default">Log in</button>
					</div>
				</div>
			</form>
		);
	}
}