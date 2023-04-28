for i in $(cat stars.txt); do
	if [ ! -d "$i" ]; then
		echo "$i" >> /home/danial/checkVR/missingAll.txt
	else
		cd "$i"
		if [ ! -f Image.png ]; then
			echo "$i" >> /home/danial/checkVR/missingImageNames.txt
		fi
		if [ ! -f Desc.txt ]; then
			echo "$i" >> /home/danial/checkVR/missingDescNames.txt
		fi
		if [ ! -f Math.txt ]; then
			echo "$i" >> /home/danial/checkVR/missingMathNames.txt
		fi
		if [ ! -f Type.png ]; then
			echo "$i" >> /home/danial/checkVR/missingTypePicNames.txt
		fi
		if [ ! -f Type.txt ]; then
			echo "$i" >> /home/danial/checkVR/missingTypeTxtNames.txt
		fi
		cd ..
	fi
done