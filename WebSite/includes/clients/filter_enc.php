<div id="filters">
			<p>
				<label for="idate">De</label> 
				<input type="date" name="idate" id="idate" placeholder="data Inicial (DD-MM-YYYY)">
				
				<label for="fdate">Até</label> 
				<input type="date" name="fdate" id="fdate" placeholder="data Final (DD-MM-YYYY)">
			</p>
			
			<p>
				<label for="estadoFacturacao">Estado Facturação</label> 
				<select name="estadoFacturacao" id="estadoFacturacao">
					<option value="">---</option>
					<option value="Pago (totalmente)">Pago (totalmente)</option>
					<option value="Pendente">Pendente</option>
					<option value="Parcial ( p/t)">Parcial ( p/t)</option>
				</select>
				

				<label for="estadoExpedicao">Estado Expedição</label> 
				<select name="estadoExpedicao" id="estadoExpedicao">
					<option value="">---</option>
					<option value="Expedido">Expedido</option>
					<option value="Pendente">Pendente</option>
				</select>
			</p>
</div>