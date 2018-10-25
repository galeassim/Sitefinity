
/**The MIT License (MIT)

Copyright (c)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

$(function () {

	$.fn.googleMap = function(params) {
		params = $.extend( {
			zoom : 10,
			coords : [48.895651, 2.290569],
			type : "ROADMAP",
			debug : false,
			langage : "english",
			overviewMapControl: false,
			streetViewControl: false,
			scrollwheel: false,
			mapTypeControl: false
		}, params);

		switch(params.type) {
			case 'ROADMAP':
			case 'SATELLITE':
			case 'HYBRID':
			case 'TERRAIN':
				params.type = google.maps.MapTypeId[params.type];
				break;
			default:
				params.type = google.maps.MapTypeId.ROADMAP;
				break;
		}

		this.each(function() {

			var map = new google.maps.Map(this, {
				zoom: params.zoom,
				center: new google.maps.LatLng(params.coords[0], params.coords[1]),
				mapTypeId: params.type,
				scrollwheel: params.scrollwheel,
				streetViewControl: params.streetViewControl,
				overviewMapControl: params.overviewMapControl,
				mapTypeControl: params.mapTypeControl

			});

			$(this).data('googleMap', map);
			$(this).data('googleLang', params.langage);
			$(this).data('googleDebug', params.debug);
			$(this).data('googleMarker', new Array());
			$(this).data('googleBound', new google.maps.LatLngBounds());
		});

		return this;
	}

	$.fn.addMarker = function(params) {
		params = $.extend( {
			coords : false,
			address : false,
			url : false,
			id : false,
			icon : false,
			draggable : false,
			title : "",
			text : "",
			success : function() {}
		}, params);

		this.each(function() {
			$this = $(this);

			if(!$this.data('googleMap')) {
				if($this.data('googleDebug'))
					console.error("jQuery googleMap : Unable to add a marker where there is no map !");
					
				return false;
			}

			if(!params.coords && !params.address) {
				if($this.data('googleDebug'))
					console.error("jQuery googleMap : Unable to add a marker if you don't tell us where !");
					
				return false;
			}

			if(params.address && typeof params.address == "string") {

				var geocodeAsync = function($that) {

					var geocoder = new google.maps.Geocoder();

					geocoder.geocode({
						address : params.address,
						bounds : $that.data('googleBound'),
						language : $that.data('googleLang')
					}, function(results, status) {

						if (status == google.maps.GeocoderStatus.OK) {
							$that.data('googleBound').extend(results[0].geometry.location);

							if(params.icon) {
								var marker = new google.maps.Marker({
									map: $this.data('googleMap'),
									position: results[0].geometry.location,
									title: params.title,
									icon: params.icon,
									draggable: params.draggable
								});
							} else {
								var marker = new google.maps.Marker({
									map: $that.data('googleMap'),
									position: results[0].geometry.location,
									title: params.title,
									draggable: params.draggable
								});
							}

							if(params.draggable) {
								google.maps.event.addListener(marker, 'dragend', function() {
									var location = marker.getPosition();

									var coords = {};

									coords.lat = location.lat();
									coords.lon = location.lng();

									params.success(coords, $this);
								});
							}

							if(params.title != "" && params.text != "" && !params.url) {
								var infowindow = new google.maps.InfoWindow({
                                    content: '<div class="map-info">' + params.title + params.text + '</div>'
                                });


                                var map = $that.data('googleMap');
                                //opens on page load
                                infowindow.open(map, marker);
								google.maps.event.addListener(marker, 'click', function() {
									infowindow.open(map, marker);
								});
							} else if(params.url) {
								google.maps.event.addListener(marker, 'click', function() {
									document.location = params.url;
								});
							}

							if(!params.id) {
								$that.data('googleMarker').push(marker);
							} else {
								$that.data('googleMarker')[params.id] = marker;
							}

							if($that.data('googleMarker').length == 1) {
								$that.data('googleMap').setCenter(results[0].geometry.location);
								$that.data('googleMap').setZoom($that.data('googleMap').getZoom());
							} else {
								$that.data('googleMap').fitBounds($that.data('googleBound'));
							}

							var coords = {};
							coords.lat = results[0].geometry.location.lat();
							coords.lon = results[0].geometry.location.lng();

							params.success(coords, $this);

						} else {
							if($this.data('googleDebug'))
								console.error("jQuery googleMap : Unable to find the place asked for the marker ("+status+")");
						}
					});
				}($this);
			} else {
				$this.data('googleBound').extend(new google.maps.LatLng(params.coords[0], params.coords[1]));

        			if(params.icon) {
					var marker = new google.maps.Marker({
						map: $this.data('googleMap'),
						position: new google.maps.LatLng(params.coords[0], params.coords[1]),
						title: params.title,
						icon: params.icon,
						draggable: params.draggable
					});
				} else {
					var marker = new google.maps.Marker({
						map: $this.data('googleMap'),
						position: new google.maps.LatLng(params.coords[0], params.coords[1]),
						title: params.title,
						draggable: params.draggable
					});
				}

        			if(params.title != "" && params.text != "" && !params.url) {
          				var infowindow = new google.maps.InfoWindow({
						content: '<div class="map-info">'+params.title+params.text + '</div>'
					});

                    
					var map = $this.data('googleMap');

                    //opens in page load   
                    infowindow.open(map, marker);

	        			google.maps.event.addListener(marker, 'click', function() {
		        			infowindow.open(map, marker);
	        			});
				} else if(params.url) {
          				google.maps.event.addListener(marker, 'click', function() {
              					document.location = params.url;
        				});
				}

				if(params.draggable) {
					google.maps.event.addListener(marker, 'dragend', function() {
						var location = marker.getPosition();

						var coords = {};

						coords.lat = location.lat();
						coords.lon = location.lng();

						params.success(coords, $this);
					});
				}

				if(!params.id) {
       					$this.data('googleMarker').push(marker);
        			} else {
        				$this.data('googleMarker')[params.id] = marker;
        			}

				if($this.data('googleMarker').length == 1) {
					$this.data('googleMap').setCenter(new google.maps.LatLng(params.coords[0], params.coords[1]));
					$this.data('googleMap').setZoom($this.data('googleMap').getZoom());
				} else {
					$this.data('googleMap').fitBounds($this.data('googleBound'));
				}

				params.success({
					lat: params.coords[0],
					lon: params.coords[1]
				}, $this);
			}
		});

		return this;
	}

	$.fn.removeMarker = function(id) {
		this.each(function() {
			var $this = $(this);

    			if(!$this.data('googleMap')) {
    				if($this.data('googleDebug'))
      					console.log("jQuery googleMap : Unable to delete a marker where there is no map !");
      					
      				return false;
    			}

    			var $markers = $this.data('googleMarker');

    			if(typeof $markers[id] != 'undefined') {
    				$markers[id].setMap(null);
    				
      				if($this.data('googleDebug'))
      					console.log('jQuery googleMap : marker deleted');
      					
      				return true;
    			} else {
      				if($this.data('googleDebug'))
      					console.error("jQuery googleMap : Unable to delete a marker if it not exists !");
      		
      				return false;
    			}
		});
	}

	$.fn.addWay = function(params) {
		params = $.extend( {
			start : false,
			end : false,
			step : [],
			route : false,
			langage : 'english'
		}, params);

		var direction = new google.maps.DirectionsService({
			region: "fr"
		});

		var way = new google.maps.DirectionsRenderer({
			draggable: true,
			map: $(this).data('googleMap'),
			panel: document.getElementById(params.route),
			provideTripAlternatives: true
		});
		
		document.getElementById.innerHTML = "";

		var waypoints = [];

    		for(var i in params.step) {
    			var step;
      			if(typeof params.step[i] == "object") {
        			step = new google.maps.LatLng(params.step[i][0], params.step[i][1]);
      			} else {
        			step = params.step[i];
      			}

      			waypoints.push({
      				location: step,
      				stopover: true
      			});
		}

		if(typeof params.end != "object") {
			var geocodeAsync = function($that) {
				var geocoder = new google.maps.Geocoder();

		    		geocoder.geocode({
		    			address  : params.end,
		    			bounds   : $that.data('googleBound'),
		    			language : params.langage
		    		}, function(results, status) {
	        			if (status == google.maps.GeocoderStatus.OK) {
	        				var request = {
	            					origin: params.start,
	            					destination: results[0].geometry.location,
	            					travelMode: google.maps.DirectionsTravelMode.DRIVING,
	            					region: "fr",
	            					waypoints: waypoints
		        			};

		        			direction.route(request, function(response, status) {
	            					if (status == google.maps.DirectionsStatus.OK) {
	              						way.setDirections(response);
	            					} else {
	              						if($that.data('googleDebug'))
	            							console.error("jQuery googleMap : Unable to find the place asked for the route ("+response+")");
	            					}
		        			});

	        			} else {
	          				if($that.data('googleDebug'))
	          					console.error("jQuery googleMap : Address not found");
	        			}
		    		});
	    		}($(this));
		} else {
			var request = {
				origin: params.start,
				destination: new google.maps.LatLng(params.end[0], params.end[1]),
				travelMode: google.maps.DirectionsTravelMode.DRIVING,
				region: "fr",
				waypoints: waypoints
			};

			direction.route(request, function(response, status) {
				if (status == google.maps.DirectionsStatus.OK) {
					way.setDirections(response);
				} else {
					if($(this).data('googleDebug'))
          					console.error("jQuery googleMap : Address not found");
				}
			});
		}

		return this;
	}
});
